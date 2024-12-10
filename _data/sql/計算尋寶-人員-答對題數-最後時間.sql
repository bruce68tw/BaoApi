
declare @baoId varchar(10)
set @baoId = 'msjEOq8ES1'
--D5V46GGVKA : test
--msjEOq8ES1 : ¥¿¦¡(·R®b´MÄ_)

select Name, Id, RightTimes=count(*), LastTime=max(LastTime)
from (
	select u.Name, u.Id, rs.StageId,
	LastTime=(
		select top 1 Created
		from dbo.StageReplyLog 
		where UserId=rs.UserId and StageId=rs.StageId
		order by Created desc
	)
	from dbo.StageReplyStatus rs
	join dbo.BaoStage s on rs.StageId=s.Id
	join dbo.UserApp u on rs.UserId=u.Id
	where s.BaoId=@baoId
	and rs.ReplyStatus='1'
) a
group by a.Name, a.Id
order by RightTimes desc, LastTime 
