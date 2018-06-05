﻿function Get-CSTypedURL {
<#
.SYNOPSIS

Lists URLs typed into the Internet Explorer URL bar.

Author: Matthew Graeber (@mattifestation)
License: BSD 3-Clause

.DESCRIPTION

Get-CSTypedURL retrieves URLs that were typed into the Internet Explorer URL bar.

.PARAMETER CimSession

Specifies the CIM session to use for this cmdlet. Enter a variable that contains the CIM session or a command that creates or gets the CIM session, such as the New-CimSession or Get-CimSession cmdlets. For more information, see about_CimSessions.

.EXAMPLE

Get-CSTypedURL

Returns all URLs typed into the Internet Explorer URL bar on a local system.

.EXAMPLE

Get-CSTypedURL -CimSession $CimSession

Returns all URLs typed into the Internet Explorer URL bar on a remote system.

.OUTPUTS

CimSweep.RegistryValue

Outputs the registry values consisting of typed IE URLs.
#>

    [CmdletBinding()]
    [OutputType('CimSweep.RegistryValue')]
    param(
        [Alias('Session')]
        [ValidateNotNullOrEmpty()]
        [Microsoft.Management.Infrastructure.CimSession[]]
        $CimSession
    )

    BEGIN {
        # If a CIM session is not provided, trick the function into thinking there is one.
        if (-not $PSBoundParameters['CimSession']) {
            $CimSession = ''
            $CIMSessionCount = 1
        } else {
            $CIMSessionCount = $CimSession.Count
        }

        $CurrentCIMSession = 0
    }

    PROCESS {
        foreach ($Session in $CimSession) {
            $ComputerName = $Session.ComputerName
            if (-not $Session.ComputerName) { $ComputerName = 'localhost' }

            # Display a progress activity for each CIM session
            Write-Progress -Id 1 -Activity 'CimSweep - Internet Explorer typed URL sweep' -Status "($($CurrentCIMSession+1)/$($CIMSessionCount)) Current computer: $ComputerName" -PercentComplete (($CurrentCIMSession / $CIMSessionCount) * 100)
            $CurrentCIMSession++

            $CommonArgs = @{}

            if ($Session.Id) { $CommonArgs['CimSession'] = $Session }

            $TypedURLs = 'SOFTWARE\Microsoft\Internet Explorer\TypedURLs'

            # Get the SIDS for each user in the registry
            $HKUSIDs = Get-HKUSID @CommonArgs

            # Iterate over each local user hive
            foreach ($SID in $HKUSIDs) {
                Get-CSRegistryValue -Hive HKU -SubKey "$SID\$TypedURLs" @CommonArgs
            }
        }
    }
}

Export-ModuleMember -Function Get-CSTypedURL