
CREATE function DecryptString
(
	@xml xml
)
returns varchar(max)
as
begin
	declare @returnString varchar(max);
	declare @temp table (line int, encryptedText varbinary(max), plainText varchar(max));

	insert into @temp (line, encryptedText, plainText)
	select t.c.value('@sequence[1]', 'int'), t.c.value('.[1]', 'varbinary(max)'), null
	from @xml.nodes('/segments/segment') t(c);

	update @temp set plainText = convert(varchar(max), decryptbykey(encryptedText));

	select	@returnString = coalesce(@returnString, '') + plainText
	from	@temp
	order by line;

	return @returnString;
end
GO


