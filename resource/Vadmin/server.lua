AddEventHandler( "playerConnecting", function(name, setReason )
	local id = GetPlayerIdentifiers(source)[1]
	if isBanned(id) then
		setReason("You have been banned on this server in " .. Duration(id) .. " Because " .. Reason(id) .. ". Contact Administrators to request Unban")
		
		CancelEvent()
		
    end
end)

-- AddEventHandler('chatMessage', function(Source, Name, Message)
-- 	local date = os.date('*t')
	
-- 	if date.day < 10 then 
-- 		date.day = '0' .. tostring(date.day) 
-- 	end
-- 	if date.month < 10 then 
-- 		date.month = '0' .. tostring(date.month) 
-- 	end
-- 	if date.hour < 10 then 
-- 		date.hour = '0' .. tostring(date.hour) 
-- 	end
-- 	if date.min < 10 then 
-- 		date.min = '0' .. tostring(date.min) 
-- 	end
-- 	if date.sec < 10 then 
-- 		date.sec = '0' .. tostring(date.sec) 
-- 	end

-- local Time = "[" .. date.day .. "/" .. date.month .. "/" .. date.year .. " " .. date.hour .. ":" .. date.min .. ":" .. date.sec .. "]"

-- 	PerformHttpRequest('http://192.168.100.126:44812/api/Chat', function(Error, Content, Head) end, 'POST', json.encode({time = Time, id = Source, name = Name, message = Message}), {['Content-Type'] = 'application/json'})

-- end)

function Reason(id)
	local reasonData = MySQL.Sync.fetchScalar("SELECT Reason FROM user_banlist WHERE SteamID = @ID", {['@ID'] = id})
-- print(reasonData)
	return reasonData	
end

function Duration(id)
	local durationData = MySQL.Sync.fetchScalar("SELECT ExpiredDate FROM user_banlist WHERE SteamID = @ID", {['@ID'] = id})
-- print(durationData)
	return durationData
end

function isBanned(id)
	local result = MySQL.Sync.fetchScalar("SELECT Banned FROM user_banlist WHERE SteamID = @ID", {['@ID'] = id})
	
	print(result)
	if result == 1 then
		print("Banned")
		return true
	end

	return false
end
