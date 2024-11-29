declare @stageName nvarchar(10)
declare @userName nvarchar(10)
set @stageName = '¤§ºq'
set @userName = 'aa'

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

select 'BaoAttend'='',* 
from dbo.BaoAttend 
where UserId=@userId
and BaoId=@baoId

select 'StageReplyLog'='',* 
from dbo.StageReplyLog 
where UserId=@userId
and StageId=@stageId
order by Created desc

select 'StageReplyStatus'='',* 
from dbo.StageReplyStatus 
where UserId=@userId
and StageId=@stageId

