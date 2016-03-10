CREATE TABLE [dbo].[Ticket_comms] (
    [Id]        INT    IDENTITY (1, 1) NOT NULL,
    [Ticket_Id] INT    NOT NULL,
    [Message]   TEXT   NOT NULL,
    [Created]   BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ticket_comms_Tickets] FOREIGN KEY ([Ticket_Id]) REFERENCES [dbo].[Tickets] ([Id])
);

