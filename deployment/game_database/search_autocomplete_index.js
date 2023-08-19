// Create the following atlas search index
// named: autocomplete
// collection: players_unique
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