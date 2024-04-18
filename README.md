# Project Title

Model Website

## Table of Contents

- [Models](#model)
- [Fans](#fans)
- [SportClubs](#sportclubs)
- [News](#news)
- [ViewModel](#viewmodel)
- [Contribution](#contribution)
- [License](#license)

## Model

Model used and its properties

- Fans
  - Id, LastName, FirstName, FullName, Birthdate
  - Subscription
- SportClubs
  - Id, Title, Fee
  - News, Subscription
- Subscription
  - SportClubId, FanId
  - Fan, SportClub
- News
  - Id, SportClubId, Url, FileName, File
  - SportClub

## Fans

Newly Added features:

- Fans/Index
  - EditSubscription link added
  - Select button added to show all the sportclubs the fan is subscribed to
- Fans/EditSubscription
  - Fan can now register and unregister to sport clubs

## SportClubs

Newly Added features:

- SportClub/Index
  - News link added
- SportClub/Delete
  - Sportclub can now only be deleted if there are no existing news 

## News

Newly Added features:

- News/Index
  - Has a list of news (images) for the selected sportclub
- News/Create
  - Allows it to add news to the list
- News/Delete
  - Allows it to remove the selected news from the list

## ViewModel

View model used:

- FanSubscriptionViewModel
- FileInputViewModel
- NewsViewModel
- SportClubSubscriptionViewModel
- SportClubViewModel

## Contribution

Amy Fujimoto and Ngoc Le

