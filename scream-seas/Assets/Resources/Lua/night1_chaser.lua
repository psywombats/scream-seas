fadeOutBGM(1)
setSwitch('night1_chaser', true)
wait(1)
playBGM('morse')
beginMessage()
message("FULFILL THINE DUTY")
wait(3)
message("OR THINE SOUL SHALT BE FORFEIT")
wait(3)
endMessage()
spawnChaser('chaser_spawn', 2.5)