CREATE SYMMETRIC KEY PSTInterOP
    WITH ALGORITHM   = AES_256
    , IDENTITY_VALUE = 'NITEMPRESA'
    , KEY_SOURCE     = 'LLAVEEMPRESA'
    ENCRYPTION BY PASSWORD = 'Pa$$w0rd1234';


--SELECT SK.name,SK.algorithm_desc,SK.create_date,SK.modify_date,SK.key_guid
--FROM sys.symmetric_keys AS SK