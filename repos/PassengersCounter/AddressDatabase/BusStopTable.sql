CREATE TABLE [dbo].[BusStopTable]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [nameBusStop] NVARCHAR(MAX) NOT NULL, 
    [coordinateBusStop] [sys].[geography] NOT NULL, 
    [countPassengers] INT NOT NULL
)
