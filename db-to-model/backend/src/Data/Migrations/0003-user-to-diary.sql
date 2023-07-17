-- add foreign key from diary to AspNetUsers
ALTER TABLE "public".diary ADD CONSTRAINT fk_diary_aspnetusers FOREIGN KEY ( user_id ) REFERENCES "public"."AspNetUsers"( "Id" )