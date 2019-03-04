
-- Troy - C:\Users\troyb\Documents\webapps\WebApps.Tests\SQLScripts\BaseScripts
-- C:\Users\troyb\Documents\webapps\WebApps.Tests\SQLScripts\BaseScripts\refresh_db.bat


:setvar path "C:\Users\troyb\Documents\VueCoreBase\VueCoreBase\SQL"

PRINT '#################################'
PRINT 'Clearing Databases'
PRINT '#################################'

PRINT 'Clearing VueBaseCore'
:r $(path)\Layout\Clear.sql
GO

PRINT '#################################'
PRINT 'Creating Databases'
PRINT '#################################'

PRINT 'Creating ASPIdentity'
:r $(path)\Layout\ASPIdentity.sql
GO
PRINT 'Creating VueCoreBase'
:r $(path)\Layout\VueCoreBase.sql
GO


PRINT '#################################'
PRINT 'Loading Databases'
PRINT '#################################'

PRINT 'Loading ASPIdentity'

:r $(path)\Data\ASPIdentitybase.sql
GO

PRINT 'Loading VueCoreBase'
:r $(path)\Data\Base.sql
GO


PRINT '#################################'
PRINT 'Complete'
PRINT '#################################'

