declare @stageName nvarchar(10)
declare @userName nvarchar(10)
--是否刪除參加及答題記錄
declare @delete bit

--set @stageName = '之歌'
set @stageName = '熊'
set @userName = 'aa'
set @delete = 0

declare @baoId varchar(10)
declare @stageId varchar(10)
declare @userId nvarchar(10)

select top 1 @baoId=BaoId, @stageId=Id
from dbo.BaoStage 
where Name like '%'+@stageName+'%'

select top 1 @userId=Id
from dbo.UserApp
where Name=@userName

print @baoId
print @stageId
print @userId

select 'UserApp'='',* 
from dbo.UserApp 
where Id=@userId

select 'Bao'='',* 
from dbo.Bao
where Id=@baoId

select 'BaoStage'='',* 
from dbo.BaoStage 
where Id=@stageId

--刪除參加記錄
if @delete = 1 begin	
	delete dbo.BaoAttend 
	where UserId=@userId
	and BaoId=@baoId
end

select 'BaoAttend'='',* 
from dbo.BaoAttend 
where UserId=@userId
and BaoId=@baoId

--刪除答題記錄
if @delete = 1 begin	
	delete a 
	from dbo.StageReplyLog a
	join dbo.BaoStage s on s.BaoId=@baoId and a.StageId=s.Id 
	where a.UserId=@userId
	--and StageId=@stageId
end

select 'StageReplyLog'='',* 
from dbo.StageReplyLog 
where UserId=@userId
and StageId=@stageId
order by Created desc

--刪除答題狀態
if @delete = 1 begin	
	delete a
	from dbo.StageReplyStatus a
	join dbo.BaoStage s on s.BaoId=@baoId and a.StageId=s.Id 
	where a.UserId=@userId
	--and StageId=@stageId
end

select 'StageReplyStatus'='',* 
from dbo.StageReplyStatus 
where UserId=@userId
and StageId=@stageId

