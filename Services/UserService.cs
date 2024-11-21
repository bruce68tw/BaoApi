using Base.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoApi.Services
{
    public class UserService
    {
        /// <summary>
        /// create UserApp
        /// </summary>
        /// <param name="row"></param>
        /// <returns>Error Msg, RECOVER, ''</returns>
        public async Task<string> CreateA(JObject row)
        {
            //check email existed
            //var row = _Str.ToJson(json["row"].ToString());
            if (row["Email"] == null)
                return _Str.GetError("Email can not Empty.");

            var email = row["Email"]!.ToString().ToLower();
            await using var db = new Db();
            var user = await GetUserByEmailA(email, db);
            if (user != null) return "RECOVER";   //for front side !!

            //create new account
            var sql = @"
insert into dbo.UserApp(Id, Name, Phone, Email, Address, 
    AuthCode, Status, Created, Revised) values(
@Id, @Name, @Phone, @Email, @Address, 
    @AuthCode, @Status, @Created, @Revised)";

            var newId = _Str.NewId();
            var now = DateTime.Now;
            var authCode = _Str.RandomStr(5, 1);
            var args = new List<object>()
            {
                "Id", newId,
                "Name", row["Name"]!.ToString(),
                "Phone", row["Phone"]!.ToString(),
                "Email", email,
                "Address", row["Address"]!.ToString(),
                "AuthCode", authCode,
                "Status", 0,
                "Created", now,
                "Revised", now,
            };
            if (await _Db.ExecSqlA(sql, args) == 0)
                return _Str.GetError("Create Failed.");

            //set authCode & send email
            user!["AuthCode"] = authCode;
            return await UpdateAndEmailA(user, true, db);
        }

        /// <summary>
        /// update UserApp
        /// </summary>
        /// <param name="row"></param>
        /// <returns>error msg if any</returns>
        public async Task<string> UpdateA(JObject row)
        {
            var sql = @"
update dbo.UserApp set
    Name=@Name, 
    Address=@Address
where Id=@Id";
            var args = new List<object>()
            {
                "Name", row["Name"]!.ToString(),
                "Address", row["Address"]!.ToString(),
                "Id", _Fun.UserId(),
            };
            return (await _Db.ExecSqlA(sql, args) == 1)
                ? "" : _Fun.SystemError;
        }

        /// <summary>
        /// 比對認証碼 for new user or recover
        /// </summary>
        /// <param name="data">encoded(authCode,email)</param>
        /// <returns>userId/error msg</returns>
        public async Task<string> AuthA(string data)
        {
            //check input
            //var cols = _Xp.Decode(data).Split(',');
            var cols = data.Split(',');
            if (cols.Length != 2)
                return _Str.GetError("Input Wrong.");

            //check email existed
            await using var db = new Db();
            var email = cols[1].ToLower();
            var user = await GetUserByEmailA(email, db);
            if (user == null)
                return _Str.GetError("Email not found.");

            //check authCode & revised time
            var now = DateTime.Now;
            if (cols[0] != user["AuthCode"]!.ToString())
                return _Str.GetError("認証碼輸入錯誤。");
            else if (_Date.MinDiff(Convert.ToDateTime(user["Revised"]), now) > 10)
                return _Str.GetError("認証碼已經失效。");

            //update UserApp.Status
            var userId = user["Id"]!.ToString();
            var sql = $@"
update dbo.UserApp set
    Status=1,
    Revised=@Revised
where Id='{userId}'
";
            await db.ExecSqlA(sql, new List<object>() { "Revised", now });

            //return encoded userId
            //return _Xp.Encode(userId);
            return userId;
        }

        /// <summary>
        /// 傳送 Recover 帳號郵件
        /// </summary>
        /// <param name="email"></param>
        /// <returns>error msg if any</returns>
        public async Task<string> EmailRecoverA(string email)
        {
            await using var db = new Db();
            var user = await GetUserByEmailA(email, db);
            return (user == null)
                ? _Str.GetError("Email not found.")
                : await UpdateAndEmailA(user, false, db);
        }

        private async Task<JObject?> GetUserByEmailA(string email, Db db)
        {
            var sql = "select * from dbo.UserApp where Email=@Email";
            return await db.GetRowA(sql, new List<object>() { "Email", email });
        }

        /// <summary>
        /// update userApp.AuthCode & 傳送郵件: new user/Recover
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isNew"></param>
        /// <param name="db"></param>
        /// <returns>error msg if any</returns>
        private async Task<string> UpdateAndEmailA(JObject user, bool isNew, Db db)
        {
            //update UserApp
            var email = user["Email"]!.ToString();
            var authCode = _Str.RandomStr(5, 1);
            var sql = $@"
update dbo.UserApp set
    AuthCode='{authCode}',
    Revised=getdate()
where Email=@Email
";
            await db.ExecSqlA(sql, new List<object>() { "Email", email });

            //send email
            user["AuthCode"] = authCode;
            if (isNew)
                await _Xp.EmailNewAuthAsync(user);
            else
                await _Xp.EmailRecoverAsync(user);

            return "";
        }

    }//class
}
