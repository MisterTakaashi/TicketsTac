CREATE TABLE [dbo].[Tickets] (
    [Id]          INT     IDENTITY (1, 1) NOT NULL,
    [Projet_Id]   INT     NOT NULL,
    [Description] TEXT    NOT NULL,
    [State]       TINYINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tickets_Projets] FOREIGN KEY ([Projet_Id]) REFERENCES [dbo].[Projets] ([Id])
);

