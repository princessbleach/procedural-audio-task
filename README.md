# Week 3 Advanced Programming 

Zoe Efstathiou

2423029


### Dev Log

For this project, I chose to demonstrate procedural audio in Unity using Wwise. This is a prototype ambience which will later be used within our FMP. The area in the FMP is a stationary tour bus in the middle of a busy, music - filled city. The player can go here to see their inventory, view collectibles and more. For the purpose of this task, there are no actual game visuals, just the procedural audio with UI to help aid understanding of what is happening. The dynamic ambience is made up of: looping music, looping crickets, sporadic car noises, crowd sounds and distant music playing. 

I bean by creating Play events for each sound in Wwise and added them to a Sound bank that was generated and loaded in Unity. Music and cricket tracks were looped within Wwise and therefore just played on start within Unity. I added a high cut EQ effect within Wwise to the cricket SFX and adjusted it accordingly to muffle the noises as if they were outside.

I then had two car SFX: car rev and car drive. I wanted these to randomly fade in and out. I created a MonoBehaviourScript in Unity to control this randomisation. The code chooses the sound and sets a random interval. This interval can be adjusted within the Unity editor, without having to go back into code. I timer is created and an if statement checks when it is finished to generate the next random interval. This creates a natural soundscape where sounds are played sporadically. I also used Wwise's randomise pitch tool to allow slight pitch variations for these sounds to increase realism. 

I also had a long crowd noises foley/SFX sample which I set to play on loop but I used another MonoBehaviourScript to randomly fade in and out its volume. This gives the illusion that crowds are walking past the player's window. The script waits for a random amount of time and then fades the crowd in, keeps it playing for a changeable amount of time and then fades the crowd out. These fade values can also be adjusted in editor. The fading works by using Lerp to interpolate between two values. In this case: 0 volume - 100 volume. I used RTPC in Wwise to connect the volume variable to the sound's volume. I also once again used a high cut EQ effect to muffle the voices slightly and added a small randomise pitch. 

My final sound was a distant club music boom which I wanted to randomly fade in and out. I used a similar C# script to wait a random amount of time and then start the music before stopping that playback instance. 

This system/prototype can be defined as procedural as it uses randomised event timing to vary when environmental sounds occur. Predictable playback is avoided. Additional sound variation is introduced through pitch changes within Wwise. This means the soundscape will never be truly the same and is changing each moment for each player. 

### AI DECLARATION

Generative AI (ChatGPT (GPT-6)) was used to help generate code, troubleshoot and bug fix.


### Sound Bibliography

Music created by myself
Sound FX downloaded license free from Freesounds.org
