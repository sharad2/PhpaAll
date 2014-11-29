--25 October 2010. Maintained by Satpal
---------------------------------

ALTER TABLE Store.GRN ALTER COLUMN Remarks NVARCHAR(2000);
ALTER TABLE Store.GRN ALTER COLUMN InvoiceNo NVARCHAR(200);
ALTER TABLE Store.GRN ALTER COLUMN DeliveryChallanNumber NVARCHAR(200);
ALTER TABLE Store.GRN ADD AmendmentNo  NVARCHAR(100);
ALTER TABLE Store.GRN ADD AmendmentDate DATETIME;


