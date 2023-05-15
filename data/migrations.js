sh.shardCollection("Leafsteroids.players_unique", {Nickname:1}, true);


db.players_unique.remove({});

db.players.find({}).forEach(player => {
  db.players_unique.insertOne({
    _id: player._id,
    Nickname: player.Nickname,
    location: player.location
  });
});

