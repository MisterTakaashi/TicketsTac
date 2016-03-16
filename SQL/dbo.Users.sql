CREATE TABLE [dbo].[Users] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Username] VARCHAR (50)  NOT NULL,
    [Email]    VARCHAR (MAX) NOT NULL,
    [Password] VARCHAR (MAX) NOT NULL,
    [Rank]     INT           NOT NULL,
    [Created]  BIGINT        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

INSERT INTO [dbo].[Users] ( Username, Email, Password, Rank, Created ) VALUES ( 'UserTest', 'Test@gmail.com', 'a94a8fe5ccb19ba61c4c0873d391e987982fbbd3', 90, 1458140337);