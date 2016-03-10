CREATE TABLE [dbo].[Projet_managers] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [Manager_Id] INT NOT NULL,
    [Projet_Id]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Projet_managers_Managers] FOREIGN KEY ([Manager_Id]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Projet_managers_Projets] FOREIGN KEY ([Projet_Id]) REFERENCES [dbo].[Projets] ([Id])
);

