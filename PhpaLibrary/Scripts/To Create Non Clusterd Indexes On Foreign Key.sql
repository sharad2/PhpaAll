--Create non clustered indexes for all foreign key references in the database
DECLARE @sql nvarchar(max) 
SELECT @sql = IsNull(@sql + ';' + char(13) , '') +  
'CREATE NONCLUSTERED INDEX [IDX_FK_' + tablename + '_' + columnname +'] ON [dbo].[' + tablename + '] ( [' + columnname + '] ASC)'
FROM 
(
    --Script all foreign key columns that are not already an index      
    SELECT
        o.name AS tablename, cols.name AS columnName FROM sys.foreign_key_columns fc
        inner join sys.objects o on fc.parent_object_id = o.object_id
        inner join sys.columns cols on cols.object_id = o.object_id and fc.parent_column_id = cols.column_id
    EXCEPT 
        SELECT o.name, cols.name  FROM sys.index_columns icols
        inner join sys.objects o on icols.object_Id = o.object_id
        inner join sys.columns cols on cols.object_id = o.object_id and icols.column_id = cols.column_id
) T
ORDER BY 
    tablename, 
    columnname

--Display the sql that will be executed
Print @sql 

--Run the sql and create all the indexes
exec sp_executesql @sql