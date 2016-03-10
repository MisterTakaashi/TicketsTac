CREATE TABLE [dbo].[Projet_operators] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [Operator_Id] INT NOT NULL,
    [Projet_Id]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Projet_operators_Operators] FOREIGN KEY ([Operator_Id]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Projet_operators_Projets] FOREIGN KEY ([Projet_Id]) REFERENCES [dbo].[Projets] ([Id])
);

