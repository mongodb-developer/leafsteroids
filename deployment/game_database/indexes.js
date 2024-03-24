db.recordings.createIndex({ "DateTime": 1 });
db.recordings.createIndex({ "location":1, "Event._id":1, "Player.Nickname": 1});
db.recordings.createIndex({ "Player.Nickname": 1,"Event._id": 1 });