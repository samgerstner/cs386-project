using UnityEngine;

public class Ranged : Item
{
    [Header( "RANGED DATA" )]

    /// <summary>
    ///     <para>
    ///         Data that will affect how this weapon behaves
    ///     </para>
    /// </summary>
    [SerializeField]
    private RangedData rangedData;


    [Header( "CURRENT STATS" )]

    /// <summary>
    ///     <para>
    ///         Shows how many bullets are in the magazine at current
    ///     </para>
    /// </summary>
    private int bulletsLeft = 0;

    /// <summary>
    ///     <para>
    ///         Identifies how many birdshot is left
    ///     </para>
    /// </summary>
    ///
    /// <remarks>
    ///     <para>
    ///         Used in shotgun simulation
    ///     </para>
    /// </remarks>
    private int shotLeft = 1;


    [Header( "PROJECTILE HEADING" )]

    /// <summary>
    ///     <para>
    ///         Heading the projectile will follow
    ///     </para>
    ///
    ///     <para>
    ///         To change between shots
    ///     </para>
    /// </summary>
    //private Vector2 heading = Vector2.zero;


    [Header( "ACTION FLAGS" )]

    /// <summary>
    ///     <para>
    ///         Identifies if this weapon is being reloaded
    ///     </para>
    /// </summary>
    private bool reloading = false;

    /// <summary>
    ///     <para>
    ///         Identifies if this weapon is to be used
    ///     </para>
    /// </summary>
    private bool shooting = false;

    [Header( "TIMERS" )]

    /// <summary>
    ///     <para>
    ///         Time until reload is finished
    ///     </para>
    /// </summary>
    private float reloadTimer = 0.000000f;

    /// <summary>
    ///     <para>
    ///         Time until next bullect can be fired
    ///     </para>
    /// </summary>
    private float fireRateTimer = 0.000000f;


    private void Awake()
    {
        reloadTimer = rangedData.reloadSpeed;
        bulletsLeft = rangedData.magazineSize;
        shotLeft = rangedData.roundsPerShot;
    }

    private void Update()
    {
        if ( equipped )
        {
            GetInput();

            if ( reloading && reloadTimer <= 0.000000f )
            {
                Reload();
            }

            if ( shooting && !reloading && bulletsLeft > 0 &&
                 fireRateTimer <= 0.000000f )
            {
                Shoot();

                bulletsLeft--;
            }
        }
    }

    /// <summary>
    ///     <para>
    ///         Listens for input and updates this weapon's state
    ///     </para>
    /// </summary>
    private void GetInput()
    {
        // decrease reload and fire rate timer
        fireRateTimer -= Time.deltaTime;

        if ( reloading )
        {
            reloadTimer -= Time.deltaTime;
        }
        else if ( bulletsLeft < rangedData.magazineSize )
        {
            reloading = Input.GetKeyDown( KeyCode.R );
        }

        if ( rangedData.isAutomatic )
        {
            shooting = Input.GetKey( KeyCode.Mouse0 );
        }
        else
        {
            shooting = Input.GetKeyDown( KeyCode.Mouse0 );
        }
    }

    /// <summary>
    ///     <para>
    ///         Simulates reloading
    ///     </para>
    /// </summary>
    private void Reload()
    {
        reloadTimer = rangedData.reloadSpeed;

        bulletsLeft = rangedData.magazineSize;

        reloading = false;
    }

    /// <summary>
    ///     <para>
    ///         Simulates shooting a bullet
    ///     </para>
    /// </summary>
    private void Shoot()
    {
        Ray shotHeading = new Ray( transform.position, transform.right );

        // figure a "spreaded" path for the bullet

        /* Built in because I will run the pros and cons of hit-scan vs.
         * projectiles by the group and see what people think
         * 
        // spawn a bullet, and add some force to it in on the determined forward
        // vector
        GameObject spawnedProjectile = Instantiate( rangedData.ammunition,
                                       transform.position, transform.rotation );

        Rigidbody2D spawnedBody = spawnedProjectile.GetComponent<Rigidbody2D>();

        spawnedBody.AddForce( spawnedProjectile.transform.forward * 200f );
        */

        Debug.DrawLine( transform.position,
                        new Vector3( transform.position.x + 100f,
                                     transform.position.y,
                                     transform.position.z - 2f ),
                        Color.cyan );

        // decrease shotLeft
        shotLeft--;

        // check for anymore shot left
        if ( shotLeft > 0 )
        {
            // shoot another
            Shoot();
        }

        // reset shotLeft
        shotLeft = rangedData.roundsPerShot;

        fireRateTimer = rangedData.fireRate;
    }
}