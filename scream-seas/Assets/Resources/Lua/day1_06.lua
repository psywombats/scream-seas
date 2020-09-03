setBG('bridge')
enterNVL()

enter('you', 'a')
enter('shawn', 'b')
enter('cy', 'd')

speak('shawn', "Hey! Cy! Cyyy!!")
speak('cy', "Shawn? I thought you'd be asleep.")
speak('shawn', "I thought I'd give our guest a tour.")
speak('cy', "Not a bad idea... They still haven't said anything?")
speak('shawn', "Silence. The little guy's earned the name.")
speak('you', "...")
speak('cy', "I wonder... You must've been something more terrible than any of us could imagine...")
speak('shawn', "You think the whole 'mute' thing is a psych-medical thing? I have a few crystals in my cabin. I'd be up for testing out their healing powers on ol' Silence here.")
speak('cy', "No crystals, Shawn.")
speak('shawn', "Maybe it's a chakra thing? You know I have some tea, too...")
speak('cy', "No crystals, and no herbal tea. Especially no herbal tea with psychoactive ingredients. I can deal with you tripping around the deck in the early morning, but if I catch wind of you giving anything remotely sketchy to our guest here - ")
speak('you', "...!")
speak('cy', "Then I'll throw you overboard myself, Shawn. Got it?")
speak('shawn', "Roger! Ten-four! You sound more like Ari every day, Cy.")
speak('cy', "Haha, Ari wouldn't have given you a warning.")
speak('shawn', "Oh, uhh, I almost forgot. I was giving Silence the tour.")
speak('shawn' "Welcome to the bridge of the Revolution! This is the natural habitat of both Cy the Sensitive, and Leo the Loud.")
speak('cy', "Oh knock it off. Leo's been awake 24 hours and if he hears you calling him that he'll decide that keelhauling is back in style and you've been asking for one.")
speak('shawn', "And Cy won't admit it but she's the brains of the outfit. Without Cy I don't think we'd be able to cover more than mile before hitting an iceberg.")
speak('cy', "We're in the mid-Atlantic in the middle of summer. The only iceberg here is the one between your ears.")
speak('shawn', "And Leo is the glue that binds us valiant crewmen together! I mean he's asleep and doesn't actually have any skills - ")
speak('cy', "Besides the skill of paying us our salary, you mean.")
speak('shawn', "Well put, well put.")
wait(1.0)
speak('cy', "Hey, you...")
speak('you', "...")
speak('cy', "Shawn might try pretty hard to play the the ship idiot, but I've known him forever and bailed him out more than once. He's just the sort of guy that doesn't know how to repay his favors.")
speak('shawn', "Slander! Come on, Silence! This is my cue -- time to leave!")
exitNVL()

wait(1.0)
play('day1_07')
