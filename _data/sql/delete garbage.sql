
/*
truncate table tmpTable
truncate table tmpColumn
*/

delete a
--select TableName='BaoAttend', a.* 
from BaoAttend a
left join Bao b on a.BaoId=b.Id
where b.Id is null

delete a
--select TableName='BaoStage',a.*
from BaoStage a
left join Bao b on a.BaoId=b.Id
where b.Id is null

delete a
--select TableName='StageReplyLog',a.*
from StageReplyLog a
left join BaoStage s on a.StageId=s.Id
where s.Id is null

delete a
--select TableName='StageReplyStatus',a.*
from StageReplyStatus a
left join BaoStage s on a.StageId=s.Id
where s.Id is null

