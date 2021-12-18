CREATE function EncryptString
(
	@plainText varchar(max),
	@encryptionKey varchar(100)
)
returns xml
as begin
	declare @break int = 5000;
	declare @encryptedSegment varbinary(max);
	declare @line int = 0;
	declare @plainSegment varchar(max);
	declare @temp table (line int, plainText varchar(max), encryptedText varbinary(max));
	declare @xml xml;

	while len(@plainText) > 0
	begin
		set @line = @line + 1;
		set @plainSegment = left(@plainText, @break);
		set @encryptedSegment = ENCRYPTBYKEY(Key_GUID(@encryptionKey), @plainSegment);
		insert into @temp (line, plainText, encryptedText) values (@line, @plainSegment, @encryptedSegment);
		set @plainText = SUBSTRING(@plainText, @break + 1, len(@plainText));
	end

	set @xml = (select line as '@sequence', encryptedText as '*' from @temp order by line for xml path ('segment'), root ('segments'));

	return @xml;
end

GO


