

DO $$
BEGIN
  IF NOT EXISTS(SELECT * FROM "Environment") THEN
		INSERT INTO "Environment"("Name", "Order")
		VALUES('Func',1);
		
		INSERT INTO "Environment"("Name", "Order")
		VALUES('Prod',1);
	END IF;
END $$;