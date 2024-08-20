ALTER TABLE [dbo].[CalendarEvents]
ADD CONSTRAINT [FK_CalendarEvents_Users] 
FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE;

ALTER TABLE [dbo].[Alerts]
DROP CONSTRAINT [FK_Alerts_CalendarEvents];

ALTER TABLE [dbo].[Alerts]
ADD CONSTRAINT [FK_Alerts_CalendarEvents] 
FOREIGN KEY ([EventID]) REFERENCES [dbo].[CalendarEvents] ([EventID])
ON DELETE CASCADE;


