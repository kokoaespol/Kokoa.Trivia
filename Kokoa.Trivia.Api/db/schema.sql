create table "T_TRIVIA_QUESTION"
(
    "TRIVIA_QUESTION_ID" serial primary key,
    "Title"              varchar(255) not null unique
);

create table "T_TRIVIA_OPTION"
(
    "TRIVIA_OPTION_ID"   serial primary key,
    "TRIVIA_QUESTION_ID" integer      not null
        references "T_TRIVIA_QUESTION" ("TRIVIA_QUESTION_ID")
            on delete cascade,
    "Content"            varchar(255) not null
);

create table "T_TRIVIA_ANSWER"
(
    "TRIVIA_QUESTION_ID" integer not null
        references "T_TRIVIA_QUESTION" ("TRIVIA_QUESTION_ID")
            on delete cascade,
    "TRIVIA_OPTION_ID"   integer not null
        references "T_TRIVIA_OPTION" ("TRIVIA_OPTION_ID")
            on delete cascade
);
