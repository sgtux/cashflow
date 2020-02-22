CREATE TABLE public."CreditCard" (
    "Id" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Description" character varying(255),
    "UserId" integer NOT NULL
);

CREATE SEQUENCE public."CreditCard_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

CREATE TABLE public."Installment" (
    "Id" integer NOT NULL,
    "PaymentId" integer NOT NULL,
    "Cost" numeric(10,2) NOT NULL,
    "Number" integer NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Paid" boolean
);

CREATE SEQUENCE public."Installment_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

CREATE TABLE public."Payment" (
    "Id" integer NOT NULL,
    "Description" character varying(1000) NOT NULL,
    "UserId" integer NOT NULL,
    "Type" integer NOT NULL,
    "CreditCardId" integer,
    "FixedPayment" boolean,
    "Invoice" boolean
);

CREATE SEQUENCE public."Payment_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

CREATE TABLE public."User" (
    "Id" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "Password" character varying(255) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);

CREATE SEQUENCE public."User_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER TABLE ONLY public."CreditCard" ALTER COLUMN "Id" SET DEFAULT nextval('public."CreditCard_id_seq"'::regclass);

ALTER TABLE ONLY public."Installment" ALTER COLUMN "Id" SET DEFAULT nextval('public."Installment_id_seq"'::regclass);

ALTER TABLE ONLY public."Payment" ALTER COLUMN "Id" SET DEFAULT nextval('public."Payment_id_seq"'::regclass);

ALTER TABLE ONLY public."User" ALTER COLUMN "Id" SET DEFAULT nextval('public."User_id_seq"'::regclass);


ALTER TABLE ONLY public."CreditCard"
    ADD CONSTRAINT "CreditCard_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Installment"
    ADD CONSTRAINT "Installment_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "Payment_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."CreditCard"
    ADD CONSTRAINT "CreditCard_userId_fkey" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");

ALTER TABLE ONLY public."Installment"
    ADD CONSTRAINT "Installment_paymentId_fkey" FOREIGN KEY ("PaymentId") REFERENCES public."Payment"("Id");

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "Payment_creditCardId_fkey" FOREIGN KEY ("CreditCardId") REFERENCES public."CreditCard"("Id");

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "Payment_userId_fkey" FOREIGN KEY ("UserId") REFERENCES public."User"("Id");