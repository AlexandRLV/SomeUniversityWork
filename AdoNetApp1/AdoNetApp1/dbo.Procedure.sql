CREATE PROCEDURE [dbo].DeleteCategory (@CategoryId INT)
AS
BEGIN
	SET @count = SELECT COUNT FROM Books WHERE Category = @CategoryId;

RETURN 0
END