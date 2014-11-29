--Script to Rebuild All indexes

DECLARE @Table VARCHAR(255)  
DECLARE @cmd NVARCHAR(1000)  
DECLARE @fillfactor INT 
SET @fillfactor = 90 

SET @cmd = 'DECLARE TableCursor CURSOR FOR SELECT table_catalog + ''.'' + table_schema + ''.'' + table_name as tableName   
                FROM INFORMATION_SCHEMA.TABLES WHERE table_type = ''BASE TABLE'''   
	
Print '--Started the rebuilding process--'
Print '--Please wait for process to complete--'
Print ''
EXEC (@cmd)  
OPEN TableCursor   

FETCH NEXT FROM TableCursor INTO @Table   
WHILE @@FETCH_STATUS = 0   
BEGIN   
   SET @cmd = 'ALTER INDEX ALL ON ' + @Table + ' REBUILD WITH (FILLFACTOR = ' + CONVERT(VARCHAR(3),@fillfactor) + ')'  
   Print 'working on: ' + @Table 
   EXEC (@cmd)  
   FETCH NEXT FROM TableCursor INTO @Table   
END   

CLOSE TableCursor   
DEALLOCATE TableCursor  

Print ''
Print '--Rebuilding process completed--'