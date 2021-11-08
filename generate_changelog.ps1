param (
  [string]$cdgitfolder,
  [Parameter(Mandatory = $true)][string]$branch,
  [string]$workitembaseuri = '{0}',
  [string]$since,
  [string]$logp
)

#$since="2021-10-01"

Write-Output "Creating Changelogs from git history using branch=$branch"

function ConcatArrayToString($array, $sep = '') {
  $line = '';
  for ($a = 0; $a -lt $array.Count; $a++) {
    $line += $array[$a] + $sep
  }
  return $line
}
function DisplayArray($arr) {
  #echo $arr.Count
  foreach ($items in $arr) {
    Write-Output $items
  }
}

#setting up git command

$gitcommand = 'git rev-list --first-parent'

if (-not [string]::IsNullOrEmpty($cdgitfolder) -and -not [string]::IsNullOrWhiteSpace($cdgitfolder)) {
  Set-Location $cdgitfolder
}
if (-not [string]::IsNullOrEmpty($since) -and -not [string]::IsNullOrWhiteSpace($since)) {
  $gitcommand = $gitcommand + ' --date=local --since=' + $since 
}
if (-not [string]::IsNullOrEmpty($branch) -and -not [string]::IsNullOrWhiteSpace($branch)) {
  $gitcommand = $gitcommand + ' ' + $branch 
}

Write-Output $gitcommand

#receive SHA-1 commits
[array] $shas = Invoke-Expression $gitcommand
#for ($i=0; $i -lt 2; $i++)
for ($i = 0; $i -lt $shas.length; $i++) {
  $sha = $shas[$i]
  $commitmessage = git show -s --format='%B' $sha
  #remove new line
  $messageOneLine = ConcatArrayToString($commitmessage)
  #echo $messageOneLine
  #extract workitems from commit message
  $workitemfound = $messageOneLine -match '(#[0-9]{6})'
    
  $workitems = New-Object System.Collections.Generic.List[string]
  $workitempayload = ''
  if ($workitemfound) {
    foreach ($key in $Matches.Keys) {
      $workitemnr = $Matches[$key]
      $workitemnr = $workitemnr.replace('#', '')
      $workitemuri = [string]::Format($workitembaseuri, $workitemnr)
      #dont add workitems twice (if its contained in the commit message multiple times)
      if ($workitems.Contains($workitemuri) -eq $false) {
        $workitems.Add($workitemuri)
      }
    }
    $workitempayload = ConcatArrayToString($workitems, ' ')
  }
  $since_author = git show -s --format='%ai;%an;' $sha
  $committitle = git show -s --pretty='format:%s' $sha
  $var = -join ($sha, ';', $since_author, $committitle, ';', $workitempayload);
  #print and write to file
  if (-not [string]::IsNullOrEmpty($logp) -and -not [string]::IsNullOrWhiteSpace($logp)) {
    $p = $logp + "changelog.txt"
  }
  else {
    $p = "changelog.txt"
  }
  Write-Output $var | Tee-Object $p -append
}