-   scripts
    
    -   variables
        
        -   set proper names, tooltips, range, and access
            
            -   private _variableName
                
            -   public VariableName
                
        -   set in code and hide in inspector if value isn’t worth changing
            
    -   remove commented blocks
        
    -   add descriptive comments to the top of functions
        
    -   split functions to single-responsibility
        
    -   remove nested “ifs”
        
    -   replace getters and public variables with public accessor variables “public string Name => _name”
        
    -   organize line order for vars and functions
        
    -   set default scriptable object references for managers
        
-   files
    
    -   add abbreviated prefixes
        
        -   type
            
        -   who’s it for
            
        -   what is it
            
        -   anim_thug_walk
            
        -   scan_city01_01
            
    -   organize by responsibility
        
        -   game events go with the object that calls them
            
        -   imagine wanting to take a folder and drag it into a new project and have everything still work
            
    -   Folder
        
        -   Scripts
            
            -   TopicA...
                
            -   TopicB...
                
        -   Events
            
        -   SOName1...
            
        -   SOName2...
            
        -   Audio
            
        -   Images
            
        -   Materials
            
        -   Shaders
            
        -   Anim
            
        -   etc...
            
-   blender objects
    
    -   name