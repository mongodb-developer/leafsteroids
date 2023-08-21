// Create the following atlas search indexes:

// ==========================
// named: default
// collection: players_unique
// ==========================
{
    "mappings": {
        "dynamic": true
    }
}

// ==========================
// named: autocomplete
// collection: players_unique
// ==========================
{
  "mappings": {
    "dynamic": true,
    "fields": {
      "_id": {
        "type": "autocomplete"
      }
    }
  }
}