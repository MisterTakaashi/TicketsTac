CREATE TABLE [dbo].[Ticket_Assignee]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Ticket_Id] INT NOT NULL, 
    [Assignee_Id] INT NULL,
	CONSTRAINT [FK_Ticket_Id] FOREIGN KEY ([Ticket_Id]) REFERENCES [dbo].[Tickets] ([Id]),
	CONSTRAINT [FK_Assignee_Id] FOREIGN KEY ([Assignee_Id]) REFERENCES [dbo].[Users] ([Id])
)
