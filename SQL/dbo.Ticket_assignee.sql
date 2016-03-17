CREATE TABLE [dbo].[Ticket_Assignee]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [Ticket_Id] INT NOT NULL, 
    [Assignee_Id] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Ticket_Id] FOREIGN KEY ([Ticket_Id]) REFERENCES [dbo].[Tickets] ([Id]),
	CONSTRAINT [FK_Assignee_Id] FOREIGN KEY ([Assignee_Id]) REFERENCES [dbo].[Users] ([Id])
)
