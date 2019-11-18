ALTER PROCEDURE [dbo].DeleteCategory (@CategoryId INT)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Books WHERE Category = @CategoryId)
		DELETE FROM Categories WHERE Id = @CategoryId;
	ELSE
	BEGIN
		IF NOT EXISTS (SELECT * FROM Categories WHERE Name = 'Default')
			INSERT INTO Categories (Name) VALUES ('Default')
		UPDATE Books
			SET Category = (SELECT TOP 1 Id FROM Categories WHERE Name = 'Default')
			WHERE Category = @CategoryId;
		IF (SELECT TOP 1 Name FROM Categories WHERE Id = @CategoryId) != 'Default'
			DELETE FROM Categories WHERE Id = @CategoryId;
	END
END