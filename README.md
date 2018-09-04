# IntelligentAgents
A repo for some code I used in the course

I am not allowed to publish the full agent code since the base code was provided from the
teacher and he does not want any specific solutions up freely available on github. 

However this package are created by myself in order to get the train a face detector. The
algorithm is based on the eigenface method (PCA to reduce the dimensionality of the 
problem). This is a fairly basic technique for face detection but we were not allowed to 
use any external library (including linear algebra packages) so I decided to instead of 
implementering a SVD algorithm which is quite tedious I decide to use a unsupervised 
learning algorithm called sangers rule which under training converges to the principal 
components of the images. 

It worked fairly good and the method recognized and could distinguish between 7-10 unique 
faces and simultaneously were able to recognize if a new person that was not added to the 
known faces did show up.

