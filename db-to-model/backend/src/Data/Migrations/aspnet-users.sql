begin;

CREATE TABLE public."AspNetRoles" (
	"Id" text NOT NULL,
	"Name" text NULL,
	"NormalizedName" text NULL,
	"ConcurrencyStamp" text NULL,
	CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);
CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");

CREATE TABLE public."AspNetUsers" (
	"Id" text NOT NULL,
	"DisplayName" text NULL,
	"Bio" text NULL,
	"FirstName" text NULL,
	"FamilyName" text NULL,
	"Address" text NULL,
	"UserName" text NULL,
	"NormalizedUserName" text NULL,
	"Email" text NULL,
	"NormalizedEmail" text NULL,
	"EmailConfirmed" bool NOT NULL,
	"PasswordHash" text NULL,
	"SecurityStamp" text NULL,
	"ConcurrencyStamp" text NULL,
	"PhoneNumber" text NULL,
	"PhoneNumberConfirmed" bool NOT NULL,
	"TwoFactorEnabled" bool NOT NULL,
	"LockoutEnd" timestamptz NULL,
	"LockoutEnabled" bool NOT NULL,
	"AccessFailedCount" int4 NOT NULL,
	CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);
CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");
CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");

CREATE TABLE public."AspNetRoleClaims" (
	"Id" int4 NOT NULL GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE),
	"RoleId" text NOT NULL,
	"ClaimType" text NULL,
	"ClaimValue" text NULL,
	CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON public."AspNetRoleClaims" USING btree ("RoleId");

CREATE TABLE public."AspNetUserClaims" (
	"Id" int4 NOT NULL GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE),
	"UserId" text NOT NULL,
	"ClaimType" text NULL,
	"ClaimValue" text NULL,
	CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_AspNetUserClaims_UserId" ON public."AspNetUserClaims" USING btree ("UserId");

CREATE TABLE public."AspNetUserLogins" (
	"LoginProvider" text NOT NULL,
	"ProviderKey" text NOT NULL,
	"ProviderDisplayName" text NULL,
	"UserId" text NOT NULL,
	CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
	CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_AspNetUserLogins_UserId" ON public."AspNetUserLogins" USING btree ("UserId");

CREATE TABLE public."AspNetUserRoles" (
	"UserId" text NOT NULL,
	"RoleId" text NOT NULL,
	CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
	CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE,
	CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");


CREATE TABLE public."AspNetUserTokens" (
	"UserId" text NOT NULL,
	"LoginProvider" text NOT NULL,
	"Name" text NOT NULL,
	"Value" text NULL,
	CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
	CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE
);

COMMIT