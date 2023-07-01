-- Add the user_id column with my own user_id as default
ALTER TABLE "public".diary ADD user_id text DEFAULT '3d523681-9967-4c97-8433-28a321747fa1' NOT NULL

-- remove the default id right after it
ALTER TABLE "public".diary ALTER COLUMN user_id DROP DEFAULT

-- add foreign key from diary to AspNetUsers
ALTER TABLE "public".diary ADD CONSTRAINT fk_diary_aspnetusers FOREIGN KEY ( user_id ) REFERENCES "public"."AspNetUsers"( "Id" )