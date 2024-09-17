create table "TriviaQuestions"
(
    "Id" integer generated always as identity primary key,
    "Title"              varchar(255) not null unique
);

create table "TriviaOptions"
(
    "Id"   integer generated always as identity primary key,
    "TriviaQuestionId" integer      not null
        references "TriviaQuestions" ("Id")
            on delete cascade,
    "Content"            varchar(255) not null
);

create table "TriviaAnswers"
(
    "TriviaQuestionId" integer not null
        references "TriviaQuestions" ("Id")
            on delete cascade,
    "TriviaOptionId"   integer not null
        references "TriviaOptions" ("Id")
            on delete cascade
);
