  IF NOT EXISTS (SELECT * FROM Environment)
  BEGIN
	INSERT INTO Environment(Name)
	VALUES('Prod')

	
	INSERT INTO Environment(Name)
	VALUES('Func')
  END
  GO