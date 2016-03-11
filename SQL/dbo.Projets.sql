CREATE TABLE [dbo].[Projets] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    [Description] TEXT         NULL,
    [Client_Id]   INT          NOT NULL,
    [Created]     BIGINT       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Projets_Clients] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Users] ([Id])
);

