
db.createView('vwEvents4GameClient', 'events', 
  [
    {
      "$project": {
        "_id": 1,
        "name": 1
    }}
  ]
);

db.createView('vwPlayers4GameClient', 'players_unique', 
  [
    {
      "$project": {
        "_id": 1,
        "Nickname": 1,
    }}
  ]
);