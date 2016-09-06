USER=sashavol
HOST=sashavol.com
PORT=2222

COMPONENT_DLL="./bin/Release/LiveSplit.Spelunky.dll"
UPDATE_XML="./update.LiveSplit.Spelunky.xml"

show_version() {
	version=`grep "version=" ${UPDATE_XML} | tail -n 1 | sed -r 's/.*version="([0-9]\.[0-9]\.[0-9])".*/\1/g'`;
	echo "Version ${version}";
}

transfer() {
	scp -P ${PORT} ${COMPONENT_DLL} ${USER}@${HOST}:~/public_html/frozlunky/autosplitter/Components; 
	scp -P ${PORT} ${UPDATE_XML} ${USER}@${HOST}:~/public_html/frozlunky/autosplitter;
}

show_version && transfer
