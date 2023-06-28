scp -i ~/.ssh/dominicfrei.pem * ubuntu@34.238.80.46:/home/ubuntu
scp -i ~/.ssh/dominicfrei.pem .env ubuntu@34.238.80.46:/home/ubuntu

ssh -i ~/.ssh/dominicfrei.pem ubuntu@34.238.80.46

sudo apt update
sudo apt -y upgrade
sudo apt install -y python3 python3-venv
cd /home/ubuntu
python3 -m venv venv
. venv/bin/activate
pip install -r requirements.txt
gunicorn --log-level=debug -b 0.0.0.0:8000 application:app
