import numpy as np
import math


def npz_data(npz):
    query = np.load(npz)
    return query['intrinsic_mat'], query['extrinsic_mat'], query['depth_map']


def generate_pointcloud(depth, intrinsic, extrinsic, scalingFactor):
    focalLengthX = intrinsic[0][0]
    focalLengthY = intrinsic[1][1]
    centerX = intrinsic[0][2]
    centerY = intrinsic[1][2]
    
    points = []    
    for v in range(depth.shape[0]):
        for u in range(depth.shape[1]):

            if depth[v,u]<=0: continue
            Z = depth[v,u] / scalingFactor
            X = (u - centerX) * Z / focalLengthX
            Y = (v - centerY) * Z / focalLengthY
            '''
            x_over_z = (centerX - u) / focalLengthX
            y_over_z = (centerY - v) / focalLengthY
            Z = depth[v,u] / scalingFactor / np.sqrt(1. + x_over_z**2 + y_over_z**2)
            if Z<=0: continue
            X = x_over_z * Z
            Y = y_over_z * Z
            '''

            '''
            Z = depth[v,u] / scalingFactor
            if Z<=0: continue
            X = (u - centerX) * Z / focalLengthX
            Y = (v - centerY) * Z / focalLengthY
            '''
            X,Y,Z = extrinsic[:,:3].T @ (np.array([X,Y,Z]).T - extrinsic[:,3])
            points.append([X, Y, Z])
    result = np.array(points)

    return result


def isRotationMatrix(R) :
    Rt = np.transpose(R)
    shouldBeIdentity = np.dot(Rt, R)
    I = np.identity(3, dtype = R.dtype)
    n = np.linalg.norm(I - shouldBeIdentity)
    return n < 1e-6


def rotationMatrixToEulerAngles(R) :

    # assert(isRotationMatrix(R))

    sy = math.sqrt(R[0,0] * R[0,0] +  R[1,0] * R[1,0])

    singular = sy < 1e-6

    if  not singular :
        x = math.atan2(R[2,1] , R[2,2])
        y = math.atan2(-R[2,0], sy)
        z = math.atan2(R[1,0], R[0,0])
    else :
        x = math.atan2(-R[1,2], R[1,1])
        y = math.atan2(-R[2,0], sy)
        z = 0

    return x, y, z
