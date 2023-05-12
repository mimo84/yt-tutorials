CREATE  TABLE "public".diary ( 
	diary_id             integer  NOT NULL  ,
	"date"               date DEFAULT CURRENT_DATE NOT NULL  ,
	CONSTRAINT pk_diary PRIMARY KEY ( diary_id )
 );

CREATE  TABLE "public".food ( 
	food_id              integer  NOT NULL  ,
	name                 varchar(100)    ,
	CONSTRAINT pk_food PRIMARY KEY ( food_id )
 );

CREATE  TABLE "public".food_amount ( 
	food_id              integer  NOT NULL  ,
	food_amount_id       integer  NOT NULL  ,
	amount_name          varchar(100)    ,
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
	"source"             varchar(100)    ,
	CONSTRAINT unq_food_amount_food_id UNIQUE ( food_id ) ,
	CONSTRAINT pk_food_amount PRIMARY KEY ( food_amount_id )
 );

CREATE  TABLE "public".food_meal ( 
	food_meal_id         integer  NOT NULL  ,
	food_id              integer  NOT NULL  ,
	meal_id              integer  NOT NULL  ,
	food_amount_id       integer  NOT NULL  ,
	amount               numeric(10,4)  NOT NULL  ,
	CONSTRAINT unq_food_meal_meal_id UNIQUE ( meal_id ) ,
	CONSTRAINT unq_food_meal_food_id UNIQUE ( food_id ) ,
	CONSTRAINT pk_food_meal PRIMARY KEY ( food_meal_id )
 );

CREATE  TABLE "public".meal ( 
	meal_id              integer  NOT NULL  ,
	diary_id             integer  NOT NULL  ,
	name                 varchar(100)    ,
	CONSTRAINT pk_meal PRIMARY KEY ( meal_id ),
	CONSTRAINT unq_meal_diary_id UNIQUE ( diary_id ) 
 );

ALTER TABLE "public".food_amount ADD CONSTRAINT fk_food_amount_food FOREIGN KEY ( food_id ) REFERENCES "public".food( food_id );

ALTER TABLE "public".food_meal ADD CONSTRAINT fk_food_meal_food_amount FOREIGN KEY ( food_amount_id ) REFERENCES "public".food_amount( food_amount_id );

ALTER TABLE "public".food_meal ADD CONSTRAINT fk_food_meal_food FOREIGN KEY ( food_id ) REFERENCES "public".food( food_id );

ALTER TABLE "public".meal ADD CONSTRAINT fk_meal_food_meal FOREIGN KEY ( meal_id ) REFERENCES "public".food_meal( meal_id );

ALTER TABLE "public".meal ADD CONSTRAINT fk_meal_diary FOREIGN KEY ( diary_id ) REFERENCES "public".diary( diary_id );

COMMENT ON COLUMN "public".food_amount.amount_name IS 'This is to help the user to know what kind of "amount" it is, is it a serving, is it based on weight.';

COMMENT ON COLUMN "public".food_meal.food_meal_id IS 'There can be multiple times the same food and food amount within the same meal';

