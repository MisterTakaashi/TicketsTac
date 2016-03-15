CREATE TABLE [dbo].[Ticket_assignee] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [User_Id]   INT NOT NULL,
    [Ticket_Id] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);