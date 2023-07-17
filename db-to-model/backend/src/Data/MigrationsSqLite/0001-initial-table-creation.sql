CREATE  TABLE "AspNetRoles" (
	"Id"                 text  NOT NULL  ,
	"Name"               text    ,
	"NormalizedName"     text    ,
	"ConcurrencyStamp"   text    ,
	CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ( "Id" )
 );

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ( "NormalizedName" );

CREATE  TABLE "AspNetUsers" (
	"Id"                 text  NOT NULL  ,
	"DisplayName"        text    ,
	"Bio"                text    ,
	"FirstName"          text    ,
	"FamilyName"         text    ,
	"Address"            text    ,
	"UserName"           text    ,
	"NormalizedUserName" text    ,
	"Email"              text    ,
	"NormalizedEmail"    text    ,
	"EmailConfirmed"     boolean  NOT NULL  ,
	"PasswordHash"       text    ,
	"SecurityStamp"      text    ,
	"ConcurrencyStamp"   text    ,
	"PhoneNumber"        text    ,
	"PhoneNumberConfirmed" boolean  NOT NULL  ,
	"TwoFactorEnabled"   boolean  NOT NULL  ,
	"LockoutEnd"         timestamptz    ,
	"LockoutEnabled"     boolean  NOT NULL  ,
	"AccessFailedCount"  integer  NOT NULL  ,
	CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ( "Id" )
 );

CREATE INDEX "EmailIndex" ON "AspNetUsers"  ( "NormalizedEmail" );

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ( "NormalizedUserName" );

CREATE  TABLE diary (
	diary_id             integer  NOT NULL  ,
	"date"               date DEFAULT CURRENT_DATE NOT NULL  ,
	user_id              text  NOT NULL  ,
	CONSTRAINT pk_diary PRIMARY KEY ( diary_id AUTOINCREMENT),
	CONSTRAINT fk_diary_aspnetusers FOREIGN KEY ( user_id ) REFERENCES "AspNetUsers"( "Id" )
 );

CREATE  TABLE food (
	food_id              integer  NOT NULL  ,
	name                 varchar  NOT NULL  ,
	CONSTRAINT pk_food PRIMARY KEY ( food_id AUTOINCREMENT )
 );

CREATE  TABLE food_amount (
	food_id              integer  NOT NULL  ,
	food_amount_id       integer  NOT NULL ,
	amount_name          varchar    ,
	amount               numeric(10,2) DEFAULT 1 NOT NULL  ,
	protein              numeric(10,4)    ,
	fat                  numeric(10,4)    ,
	carbohydrates        numeric(10,4)    ,
	fiber                numeric(10,4)    ,
	alcohol              numeric(10,4)    ,
	sugar                numeric(10,4)    ,
	saturated_fats       numeric(10,4)    ,
	sodium               numeric(10,4)    ,
	cholesterol          numeric(10,4)    ,
	potassium            numeric(10,4)    ,
	iron                 numeric(10,4)    ,
	calcium              numeric(10,4)    ,
	"source"             varchar    ,
	CONSTRAINT pk_food_amount PRIMARY KEY ( food_amount_id AUTOINCREMENT),
	CONSTRAINT fk_food_amount_food FOREIGN KEY ( food_id ) REFERENCES food( food_id )
 );

CREATE  TABLE meal (
	meal_id              integer  NOT NULL ,
	diary_id             integer  NOT NULL  ,
	name                 varchar    ,
	CONSTRAINT pk_meal PRIMARY KEY ( meal_id AUTOINCREMENT),
	CONSTRAINT fk_meal_diary FOREIGN KEY ( diary_id ) REFERENCES diary( diary_id )
 );

CREATE  TABLE "AspNetRoleClaims" (
	"Id"                 integer  NOT NULL ,
	"RoleId"             text  NOT NULL  ,
	"ClaimType"          text    ,
	"ClaimValue"         text    ,
	CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ( "Id" ),
	CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ( "RoleId" ) REFERENCES "AspNetRoles"( "Id" ) ON DELETE CASCADE
 );

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims"  ( "RoleId" );

CREATE  TABLE "AspNetUserClaims" (
	"Id"                 integer  NOT NULL ,
	"UserId"             text  NOT NULL  ,
	"ClaimType"          text    ,
	"ClaimValue"         text    ,
	CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ( "Id" ),
	CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ( "UserId" ) REFERENCES "AspNetUsers"( "Id" ) ON DELETE CASCADE
 );

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims"  ( "UserId" );

CREATE  TABLE "AspNetUserLogins" (
	"LoginProvider"      text  NOT NULL  ,
	"ProviderKey"        text  NOT NULL  ,
	"ProviderDisplayName" text    ,
	"UserId"             text  NOT NULL  ,
	CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ( "LoginProvider", "ProviderKey" ),
	CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ( "UserId" ) REFERENCES "AspNetUsers"( "Id" ) ON DELETE CASCADE
 );

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins"  ( "UserId" );

CREATE  TABLE "AspNetUserRoles" (
	"UserId"             text  NOT NULL  ,
	"RoleId"             text  NOT NULL  ,
	CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ( "UserId", "RoleId" ),
	CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ( "RoleId" ) REFERENCES "AspNetRoles"( "Id" ) ON DELETE CASCADE  ,
	CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ( "UserId" ) REFERENCES "AspNetUsers"( "Id" ) ON DELETE CASCADE
 );

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles"  ( "RoleId" );

CREATE  TABLE "AspNetUserTokens" (
	"UserId"             text  NOT NULL  ,
	"LoginProvider"      text  NOT NULL  ,
	"Name"               text  NOT NULL  ,
	"Value"              text    ,
	CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ( "UserId", "LoginProvider", "Name" ),
	CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ( "UserId" ) REFERENCES "AspNetUsers"( "Id" ) ON DELETE CASCADE
 );

CREATE  TABLE food_meal (
	food_meal_id         integer  NOT NULL ,
	food_id              integer  NOT NULL  ,
	meal_id              integer  NOT NULL  ,
	food_amount_id       integer  NOT NULL  ,
	consumed_amount      numeric(10,4)  NOT NULL  ,
	CONSTRAINT pk_food_meal PRIMARY KEY ( food_meal_id AUTOINCREMENT),
	CONSTRAINT fk_food_meal_food FOREIGN KEY ( food_id ) REFERENCES food( food_id )   ,
	CONSTRAINT fk_food_meal_food_amount FOREIGN KEY ( food_amount_id ) REFERENCES food_amount( food_amount_id )   ,
	CONSTRAINT fk_food_meal_meal FOREIGN KEY ( meal_id ) REFERENCES meal( meal_id )
 );
