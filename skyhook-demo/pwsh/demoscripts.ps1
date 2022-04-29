#Check if the MediaValet Powershell module is available
Get-Module -Name MediaValet* -ListAvailable

#Connect to the DAM Library
Connect-MvDamAccount -Username "jeanadmin@mediavalet.net" -Region "mv-cato" 

#Display context  
Get-MvDamContext 

#Demo: Get category by path
$category = Get-MvDamCategory '\Library\Images'

#Demo: Get assets in category
Get-MvDamAssetInfo -CategoryId $category.Id | Format-Table

#Demo: SkyHOOK management lifecycle
Get-MvSkyEventType

$skySubs = New-MvSkySubscription -EndpointType Webhook -EndpointUri 'https://ngrok-mvlabs.mediavalet.net/api/events' -EventTypes @('Asset.MediaFileAdded') 

Get-MvSkySubscription

Remove-MvSkySubscription -SubscriptionId $skySubs.SubscriptionId
