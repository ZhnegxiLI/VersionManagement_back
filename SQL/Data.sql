  IF NOT EXISTS (SELECT * FROM Environment)
  BEGIN
	INSERT INTO Environment(Name, [Order])
	VALUES('Prod'2)

	
	INSERT INTO Environment(Name, [Order])
	VALUES('Func',1)
  END
  GO