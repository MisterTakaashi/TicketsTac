CREATE TABLE [dbo].[Users] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Username] VARCHAR (50)  NOT NULL,
    [Email]    VARCHAR (MAX) NOT NULL,
    [Password] VARCHAR (MAX) NOT NULL,
    [Rank]     INT           NOT NULL,
    [Created]  BIGINT        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

